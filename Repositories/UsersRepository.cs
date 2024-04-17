using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.Bill;
using Models.DTOs.Login;
using Models.DTOs.Service;
using Models.DTOs.User;
using Models.Entities;
using Repositories.Interfaces;
using Repositories.Utils.PasswordHasher;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using Utils.Enum;
using Utils.Middleware;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ManagementServiceContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UsersRepository(ManagementServiceContext context, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserWithTokenDTO> Login(UserLoginDTO userLoginDTO)
        {
            var searchedUser = await _context.Users.Where(u => 
                                                 u.Email == userLoginDTO.Email)
                                             .FirstOrDefaultAsync();

            if (searchedUser == null)
                throw new KeyNotFoundException("El email ingresado no tiene una cuenta asociada. Por favor comuníquese con su Agente asignado para dar de alta su cuenta.");
            
            if (!_passwordHasher.Verify(searchedUser.Password, userLoginDTO.Password))
                throw new BadRequestException("La contraseña ingresada no es correcta.");

            /*if (searchedUser.Active == false)
                throw new BadRequestException("Este usuario no se encuentra Activo.");*/

            return _mapper.Map<UserWithTokenDTO>(searchedUser);
        }

        public async Task<bool> ChangePassword(UserUpdatePasswordDTO userUpdatePassDTO)
        {
            User searchedUser = await _context.Users.Where(u =>
                                                 u.Email == userUpdatePassDTO.Email)
                                             .FirstOrDefaultAsync();

            if (searchedUser == null)
                throw new KeyNotFoundException("El email ingresado no tiene una cuenta asociada. Por favor comuníquese con su Agente asignado para dar de alta su cuenta.");

            if (!_passwordHasher.Verify(searchedUser.Password, userUpdatePassDTO.OldPassword))
                throw new BadRequestException("La contraseña ingresada no es correcta.");

            List<string> passwordErrors = VerifyPassword(userUpdatePassDTO.OldPassword, userUpdatePassDTO.NewPassword);
            if (passwordErrors.Any())
            {
                throw new BadRequestException("La contraseña ingresada no cumple los requisitos: " + string.Join(" ", passwordErrors));
            }

            // le cambiamos la contraseña, primero haciendole un Hash
            searchedUser.Password = _passwordHasher.Hash(userUpdatePassDTO.NewPassword);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            List<User> users = await _context.Users.Include(u => u.Address)
                                            .ThenInclude(a => a.Location)
                                            .ThenInclude(l => l.District)
                                            .ToListAsync();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("La lista de usuarios está vacía.");
            }

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetActiveUsers()
        {
            List<User> users = await _context.Users.Where(x => x.Active == true)
                                            .Include(u => u.Address)
                                            .ThenInclude(a => a.Location)
                                            .ThenInclude(l => l.District)
                                            .ToListAsync();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("La lista de usuarios activos está vacía.");
            }

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetAllAgent()
        {
            var listAgent = await _context.Users.Where(x=> x.RoleId == (int)UserRoleEnum.Agent)
                                            .Include(u => u.Address)
                                            .ThenInclude(a => a.Location)
                                            .ThenInclude(l => l.District)
                                            .ToListAsync();

            if (listAgent.Count == 0)
            {
                throw new KeyNotFoundException("No se encontraron agentes en el sistema");
            }

            return _mapper.Map<List<UserDTO>>(listAgent);
        }

        public async Task<int> GetRoleById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if(user == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con rol id igual a :{id}");
            }

            if(user.Active == false)
            {
                throw new BadRequestException("Este usuario no se encuentra Activo.");
            }

            return user.RoleId;
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _context.Users.Include(u => u.Address)
                                           .ThenInclude(a => a.Location)
                                           .ThenInclude(l => l.District)
                                            .FirstOrDefaultAsync(x => x.UserId == id);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetUsersByDistrictId(int districtId)
        {
            var district = await _context.Districts
                                        .Include(d => d.Locations)
                                        .ThenInclude(l => l.Addresses)
                                        .ThenInclude(a => a.Users)
                                        .FirstOrDefaultAsync(d => d.DistrictId == districtId);

            if (district == null)
            {
                throw new KeyNotFoundException($"No se encontró el distrito con ID {districtId}.");
            }

            var users = district.Locations
                                .SelectMany(l => l.Addresses)
                                .SelectMany(a => a.Users)
                                .ToList();

            if (users.Count == 0)
            {
                throw new KeyNotFoundException("No se encontraron usuarios en el distrito especificado.");
            }

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<AgentDTO> GetAgentsWithDistrict(int agentId)
        {
            var user = await _context.Users
                                    .Include(d => d.Districts)
                                    .FirstOrDefaultAsync(x => x.UserId == agentId);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }

            if (user.RoleId != (int)UserRoleEnum.Agent)
            {
                throw new BadRequestException("El usuario no posee rol de agente.");
            }

            if (user.Active == false)
            {
                throw new BadRequestException("Este usuario no se encuentra Activo.");
            }

            return _mapper.Map<AgentDTO>(user);
        }

        public async Task<UserWithServicesDTO> SubscribeUserToService(int userId, int serviceId)
        {
            var user = await _context.Users.Include(u => u.Address)
                                            .ThenInclude(a => a.Location)
                                            .ThenInclude(l => l.District)
                                            .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }
            if (user.Address.Location == null)
            {
                throw new KeyNotFoundException("El usuario no tiene una locación asignada.");
            }
            if (user.Address.Location.District == null)
            {
                throw new KeyNotFoundException("El usuario no tiene distrito asignado.");
            }
            if (user.Active == false)
            {
                throw new BadRequestException("Este usuario no se encuentra Activo.");
            }

            var service = await _context.Services.FindAsync(serviceId);

            if (service == null)
            {
                throw new KeyNotFoundException("No se encontró el servicio.");
            }
            if (service.Active == false)
            {
                throw new KeyNotFoundException("Este servicio se encuentra deshabilitado.");
            }

            int? districtId = user.Address.Location.DistrictId;
            var districtXservice = await _context.DistrictXservices.FirstOrDefaultAsync(
                                                        dxs => dxs.DistrictId == districtId
                                                        && dxs.ServiceId == serviceId);

            if (districtXservice == null || districtXservice.Active == false)
            {
                throw new KeyNotFoundException("Este servicio no se encuentra disponible para este usuario.");
            }

            var existingSubscription = await _context.ServiceSubscriptions.FirstOrDefaultAsync(
                                                        s => s.UserId == userId
                                                        && s.DistrictXserviceId == districtXservice.DistrictXserviceId);

            if (existingSubscription != null)
            { // Ya está suscrito:
                return await GetUserWithServicesById(userId);
            }

            // No está suscrito. Creando la suscripción:
            ServiceSubscription subscription = new ServiceSubscription()
            {
                UserId = userId,
                DistrictXserviceId = districtXservice.DistrictXserviceId,
                StartDate = DateTime.Now,
                PauseSubscription = false,
            };

            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();

            return await GetUserWithServicesById(userId);
        }

        public async Task<UserWithServicesDTO> PauseSubscribeUserToService(int subscriptionId)
        {
            var subscription = await _context.ServiceSubscriptions.FirstOrDefaultAsync(idS => idS.SubscriptionId == subscriptionId);

            if (subscription == null)
            {
                throw new KeyNotFoundException("No se encontró ninguna suscripción con el ID indicado.");
            }

            if(subscription.UserId == null)
            {
                throw new KeyNotFoundException("La suscripción no está relacionada a ningun usuario.");
            }

            if (subscription.PauseSubscription == true)
            {
                throw new BadRequestException("La suscripción del ID ingresado ya está pausada.");
            }

            // Chequeamos si es agente? si es su suscripción?

            subscription.PauseSubscription = true;

            _context.Entry(subscription)
                .Property(x => x.PauseSubscription).IsModified = true;
            await _context.SaveChangesAsync();

            return await GetUserWithServicesById((int)subscription.UserId);
        }

        public async Task<List<UserWithServicesDTO>> GetUsersWithServices()
        {
            var usersWithServicesDTO = new List<UserWithServicesDTO>();

            var users = await _context.Users
                                       .Include(u => u.Address)
                                       .ThenInclude(a => a.Location)
                                       .ThenInclude(l => l.District)
                                       .ToListAsync();

            foreach (var user in users)
            {
                var userWithServicesDTO = _mapper.Map<UserWithServicesDTO>(user);

                var subscriptionQueryResult = await _context.ServiceSubscriptions
                                                            .Include(s => s.DistrictXservice)
                                                            .ThenInclude(dxs => dxs.Service)
                                                            .Where(x => x.UserId == user.UserId
                                                                && x.DistrictXservice.Active == true
                                                                && x.DistrictXservice.Service.Active == true)
                                                            .ToListAsync();

                foreach (var subscription in subscriptionQueryResult)
                {
                    ServiceDTO serviceDTO = new ServiceDTO()
                    {
                        ServiceId = subscription.DistrictXservice.Service.ServiceId,
                        ServiceTypeId = subscription.DistrictXservice.Service.ServiceTypeId,
                        ServiceName = subscription.DistrictXservice.Service.ServiceName,
                        PricePerUnit = subscription.DistrictXservice.Service.PricePerUnit,
                        Active = subscription.DistrictXservice.Service.Active,
                    };

                    ServiceSubscriptionDTO serviceSubscriptionDTO = new ServiceSubscriptionDTO()
                    {
                        SubscriptionId = subscription.SubscriptionId,
                        UserId = user.UserId,
                        DistrictXservice = _mapper.Map<DistrictXserviceDTO>(subscription.DistrictXservice),
                        StartDate = subscription.StartDate,
                        PauseSubscription = subscription.PauseSubscription,
                        Service = serviceDTO
                    };
                    userWithServicesDTO.ServiceSubscriptions.Add(serviceSubscriptionDTO);
                }

                usersWithServicesDTO.Add(userWithServicesDTO);
            }

            return usersWithServicesDTO;
        }

        public async Task<UserWithServicesDTO> GetUserWithServicesById(int userId)
        {
            var user = await _context.Users.Include(u => u.Address)
                                           .ThenInclude(a => a.Location)
                                           .ThenInclude(l => l.District)
                                           .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new KeyNotFoundException("No se encontró el usuario.");

            if (user.Active == false)
            {
                throw new BadRequestException("Este usuario no se encuentra Activo.");
            }

            var userWithServicesDTO = _mapper.Map<UserWithServicesDTO>(user);
            var subscriptionQueryResult = await _context.ServiceSubscriptions
                                                        .Include(s => s.DistrictXservice)
                                                        .ThenInclude(dxs => dxs.Service)
                                                        .Where(x => x.UserId == userId
                                                            && x.DistrictXservice.Active == true
                                                            && x.DistrictXservice.Service.Active == true)
                                                        .ToListAsync();

            foreach (var subscription in subscriptionQueryResult)
            {
                ServiceDTO serviceDTO = new ServiceDTO()
                {
                    ServiceId = subscription.DistrictXservice.Service.ServiceId,
                    ServiceTypeId = subscription.DistrictXservice.Service.ServiceTypeId,
                    ServiceName = subscription.DistrictXservice.Service.ServiceName,
                    PricePerUnit = subscription.DistrictXservice.Service.PricePerUnit,
                    Active = subscription.DistrictXservice.Service.Active,
                };

                ServiceSubscriptionDTO serviceSubscriptionDTO = new ServiceSubscriptionDTO()
                {
                    SubscriptionId = subscription.SubscriptionId,
                    UserId = userId,
                    DistrictXservice = _mapper.Map<DistrictXserviceDTO>(subscription.DistrictXservice),
                    StartDate = subscription.StartDate,
                    PauseSubscription = subscription.PauseSubscription,
                    Service = serviceDTO
                };
                userWithServicesDTO.ServiceSubscriptions.Add(serviceSubscriptionDTO);
            }

            return userWithServicesDTO;
        }

        public async Task<ServiceSubscriptionWithUserDTO> GetSubscription(int subscriptionId)
        {
            var subscriptionQuery = await _context.ServiceSubscriptions
                                                    .Include(sub => sub.User)
                                                    .Include(sub => sub.DistrictXservice)
                                                    .ThenInclude(dxs => dxs.Service)
                                                    .Where(x => x.SubscriptionId == subscriptionId)
                                                    .FirstOrDefaultAsync();

            if (subscriptionQuery == null)
                throw new KeyNotFoundException("No se encontró la suscripción.");

            ServiceSubscriptionWithUserDTO subscription = _mapper.Map<ServiceSubscriptionWithUserDTO>(subscriptionQuery);
            subscription.Service = _mapper.Map<ServiceDTO>(subscriptionQuery.DistrictXservice.Service);
            subscription.User = await GetUserById(subscription.User.UserId);

            return _mapper.Map<ServiceSubscriptionWithUserDTO>(subscription);
        }

        public async Task<ConsumptionDTO> GetRandomSubscriptionConsumption(int subscriptionId)
        {
            ServiceSubscriptionWithUserDTO subscription = await GetSubscription(subscriptionId);

            if (subscription.PauseSubscription)
                throw new UnavailableServiceException("La suscripción está pausada actualmente.");

            if (!subscription.DistrictXservice.Active)
                throw new UnavailableServiceException("El servicio no se encuentra disponible para este distrito actualmente.");

            if (!subscription.Service.Active)
                throw new UnavailableServiceException("El servicio no se encuentra disponible en este momento.");

            // Se calcula la cantidad de días a cobrar:
            DateTime firstDayCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime firstDate = subscription.StartDate < firstDayCurrentMonth ? firstDayCurrentMonth : subscription.StartDate;
            TimeSpan daysOfConsumptionSpan = DateTime.Today - firstDate;
            int daysOfConsumption = (int)daysOfConsumptionSpan.TotalDays;

            // Se genera un número random de consumo (igual para todos los días):
            Random random = new Random();
            float dailyConsumption = (float)(random.NextDouble() * 10);

            // Calculo el costo total de consumo:
            float totalConsumption = daysOfConsumption * dailyConsumption;

            ConsumptionDTO consumptionDTO = new ConsumptionDTO()
            {
                DaysOfConsumption = daysOfConsumption,
                UnitsConsumed = totalConsumption,
                TotalCost = (float)(totalConsumption * subscription.Service.PricePerUnit),
                ServiceSubscription = subscription
            };

            return consumptionDTO;
        }

        public async Task<ConsumptionBillDTO> GenerateBill(int userId)
        {
            UserWithServicesDTO user = await GetUserWithServicesById(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }

            if (user.ServiceSubscriptions == null || !user.ServiceSubscriptions.Any())
            {
                throw new KeyNotFoundException("El usuario no está subscrito a ningún servicio.");
            }

            DateTime today = DateTime.Today;
            ConsumptionBill existingConsumptionBill = await _context.ConsumptionBills
                                                                    .FirstOrDefaultAsync(cb => cb.UserId == userId &&
                                                                                                cb.BillDate.Year == today.Year &&
                                                                                                cb.BillDate.Month == today.Month);

            if (existingConsumptionBill != null)
            {
                throw new BadRequestException("Ya existe una factura creada este mes para este usuario.");
            }

            List<BillDetailDTO> billDetails = new List<BillDetailDTO>();

            // Sumar para obtener el Total
            double total = 0;

            ConsumptionBill consumptionBill = new ConsumptionBill
            {
                UserId = userId,
                BillStatusId = 2, // El estado inicial de la factura es id = 2 'Pendiente'
                BillDate = DateTime.Now,
                Total = 0
            };

            foreach (var subscription in user.ServiceSubscriptions)
            {
                if (subscription.PauseSubscription)
                    continue;

                // Obtenemos la consumición del servicio al que está suscripto el cliente
                ConsumptionDTO consumptionSubscription = await GetRandomSubscriptionConsumption(subscription.SubscriptionId);

                var newBillDetail = new BillDetailDTO
                {
                    Subscription = subscription,
                    ConsumptionBillId = 0, // Como aún no se ha creado la factura completa, mantenemos el valor en cero
                    UnitsConsumed = consumptionSubscription.UnitsConsumed,
                    DaysBilled = consumptionSubscription.DaysOfConsumption,
                    PricePerUnit = subscription.Service.PricePerUnit
                };

                total += consumptionSubscription.UnitsConsumed * subscription.Service.PricePerUnit;
                billDetails.Add(newBillDetail);
            }

            // Calcular el total de la factura sumando los detalles de la factura
            consumptionBill.Total = total;

            _context.ConsumptionBills.Add(consumptionBill);
            await _context.SaveChangesAsync();

            foreach (var billDetail in billDetails)
            {
                billDetail.ConsumptionBillId = consumptionBill.ConsumptionBillId;
            }

            // Antes de guardar los detalles de la factura en la base de datos
            var billDetailEntities = billDetails.Select(bd => _mapper.Map<BillDetail>(bd)).ToList();

            _context.BillDetails.AddRange(billDetailEntities);
            await _context.SaveChangesAsync();

            ConsumptionBillDTO billToReturn = _mapper.Map<ConsumptionBillDTO>(consumptionBill);
            billToReturn.User = _mapper.Map<UserDTO>(user);
            billToReturn.BillDetails = billDetails;

            return billToReturn;
        }

        public async Task<ConsumptionBillDTO> UpdateBillStatus(int billId, int statusId)
        {
            ConsumptionBill bill = await _context.ConsumptionBills.FindAsync(billId);

            if (bill == null)
                throw new KeyNotFoundException($"La factura de Id: {billId} no existe.");

            BillStatus billStatus = await _context.BillStatuses.FindAsync(statusId);

            if (billStatus == null)
                throw new KeyNotFoundException($"El estado de Id: {statusId} no existe.");

            bill.BillStatusId = statusId;
            await _context.SaveChangesAsync();

            return await GetBillById(billId);
        }

        public async Task<ConsumptionBillDTO> GetBillById(int billId)
        {
            ConsumptionBill consumptionBill = await _context.ConsumptionBills.Include(cb => cb.BillDetails)
                                                                    .ThenInclude(bd => bd.Subscription)
                                                                    .ThenInclude(sub => sub.DistrictXservice)
                                                                    .ThenInclude(dxs => dxs.Service)
                                                                    .Include(cb => cb.User)
                                                                    .ThenInclude(u => u.Address)
                                                                    .ThenInclude(a => a.Location)
                                                                    .ThenInclude(l => l.District)
                                                                    .FirstOrDefaultAsync(cb => cb.ConsumptionBillId == billId);
            
            if (consumptionBill == null)
            {
                throw new KeyNotFoundException("No se pudo encontrar la factura.");
            }

            ConsumptionBillDTO result = _mapper.Map<ConsumptionBillDTO>(consumptionBill);

            return result;
        }

        public async Task<List<ConsumptionBillDTO>> GetAllBills()
        {
            List<ConsumptionBill> allBills = await _context.ConsumptionBills.Include(cb => cb.BillDetails)
                                                                    .Include(cb => cb.User)
                                                                    .ThenInclude(u => u.Address)
                                                                    .ThenInclude(a => a.Location)
                                                                    .ThenInclude(l => l.District)
                                                                    .ToListAsync();

            return _mapper.Map<List<ConsumptionBillDTO>>(allBills);
        }

        public async Task<List<ConsumptionBillDTO>> GetBillsByUserId(int userId)
        {
            List<ConsumptionBill> userBills = await _context.ConsumptionBills.Include(cb => cb.BillDetails)
                                                                    .Include(cb => cb.User)
                                                                    .ThenInclude(u => u.Address)
                                                                    .ThenInclude(a => a.Location)
                                                                    .ThenInclude(l => l.District)
                                                                    .Where(cb => cb.UserId == userId)
                                                                    .ToListAsync();

            return _mapper.Map<List<ConsumptionBillDTO>>(userBills);
        }

        public async Task<UserDTO> PostUser(UserCreationDTO userCreationDTO , int userRole, string token)
        {
            var rol = GetRolesFromToken(token);
            var clientRoleValue = Convert.ToInt32(UserRoleEnum.Client).ToString();
            var agentRoleValue = Convert.ToInt32(UserRoleEnum.Agent).ToString();

            if (rol.Contains(clientRoleValue) || (rol.Contains(agentRoleValue) && userCreationDTO.RoleId == (int)UserRoleEnum.Admin))
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para acceder a este recurso.");
            }

            if (ExistsDniUser(userCreationDTO.Dninumber).Result)
            {
                throw new KeyNotFoundException($"El DNI ingresado ya existe, no puede crear un usuario con DNI: {userCreationDTO.Dninumber}");
            }

            if (ExistsEmailUser(userCreationDTO.Email).Result)
            {
                throw new KeyNotFoundException($"El Email ingresado ya existe, no puede crear un usuario con Email: {userCreationDTO.Email}");
            }

            if (!ExistsUserRole(userCreationDTO.RoleId).Result)
            {
                throw new KeyNotFoundException($"No se puede crear un usuario con un Id Role que no existe en el sistema: {userCreationDTO.RoleId}");
            }

            var location = await _context.Locations.Where(x => x.PostalCode == userCreationDTO.PostalCode && x.DistrictId == userCreationDTO.DistrictId).FirstOrDefaultAsync();

            if (location == null)
            {
                throw new KeyNotFoundException($"No se encontró localidad con el codigo postal: {userCreationDTO.PostalCode}");
            }

            var addres = new Address()
            {
                StreetName = userCreationDTO.StreetName,
                Neighborhood = userCreationDTO.Neighborhood,
                StreetNumber = userCreationDTO.StreetNumber,
                LocationId = location.LocationId
            };

            await _context.AddAsync(addres);
            await _context.SaveChangesAsync();

            var user = new User() 
            {
                RoleId = userRole,
                AddressId = addres.AddressId,
                FirstName = userCreationDTO.FirstName,
                LastName = userCreationDTO.LastName,
                Email = userCreationDTO.Email,
                Dninumber = userCreationDTO.Dninumber,
                Password = _passwordHasher.Hash(userCreationDTO.Password),
                CreationDate = DateTime.Now,
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            UserDTO userDTO = await GetUserById(user.UserId);
            return userDTO;
        }

        public async Task<UserDTO> UpdateUser(UserUpdateDTO userUpdateDTO, string token)
        {
            var rol = GetRolesFromToken(token);
            var clientRoleValue = Convert.ToInt32(UserRoleEnum.Client).ToString();

            if (rol.Contains(clientRoleValue))
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para acceder a este recurso.");
            }

            var existingUser = await _context.Users.FindAsync(userUpdateDTO.UserId);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("No se encontró un usuario con el Id ingresado.");
            }

            if (existingUser.Active == false)
            {
                throw new BadRequestException("Este usuario no se encuentra Activo.");
            }

            existingUser.FirstName = userUpdateDTO.FirstName;
            existingUser.LastName = userUpdateDTO.LastName;
            existingUser.Email = userUpdateDTO.Email;

            await _context.SaveChangesAsync();

            return await GetUserById(existingUser.UserId);
        }

        public async Task<UserDTO> DeleteUser(int id, string token)
        {
            var rol = GetRolesFromToken(token);
            var adminRoleValue = Convert.ToInt32(UserRoleEnum.Admin).ToString();

            if (!rol.Contains(adminRoleValue))
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos para acceder a este recurso.");
            }

            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("No se encontró un usuario con el Id ingresado.");
            }

            if (existingUser.Active == false)
            {
                throw new BadRequestException("Este usuario no se encuentra Activo.");
            }

            existingUser.Active = false;
            await _context.SaveChangesAsync();

            return await GetUserById(existingUser.UserId);
        }

        private async Task<bool> ExistsDniUser(string dni)
        {
            return await _context.Users.AnyAsync(x=> x.Dninumber == dni);
        }
        private async Task<bool> ExistsEmailUser(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
        private async Task<bool> ExistsUserRole(int role)
        {
            return await _context.UserRoles.AnyAsync(x => x.RoleId == role);
        }
        private List<string> VerifyPassword(string oldpass, string newpass)
        {
            List<string> errors = new List<string>();

            // Verificar que la nueva contraseña y la vieja no sean iguales
            if (oldpass == newpass)
                errors.Add("La nueva contraseña no puede ser igual a la contraseña anterior.");

            // Verificar longitud de la nueva contraseña (entre 8 y 16 caracteres)
            if (newpass.Length < 8 || newpass.Length > 16)
                errors.Add("La contraseña debe tener entre 8 y 16 caracteres.");

            // Verificar si la nueva contraseña contiene caracteres alfanuméricos
            bool hasAlphanumeric = false;
            foreach (char c in newpass)
            {
                if (char.IsLetterOrDigit(c))
                {
                    hasAlphanumeric = true;
                    break;
                }
            }
            if (!hasAlphanumeric)
                errors.Add("La contraseña debe contener al menos un carácter alfanumérico.");

            // Verificar si la nueva contraseña contiene al menos un carácter en mayúscula y uno en minúscula
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            foreach (char c in newpass)
            {
                if (char.IsUpper(c))
                    hasUpperCase = true;
                if (char.IsLower(c))
                    hasLowerCase = true;
            }
            if (!hasUpperCase || !hasLowerCase)
                errors.Add("La contraseña debe contener al menos una letra mayúscula y una minúscula.");

            // Verificar si la nueva contraseña contiene al menos un carácter especial
            string specialCharacters = @"!@#$%^&*()-_+=[]{}|;:'<>,.?/";
            bool hasSpecialCharacter = false;
            foreach (char c in newpass)
            {
                if (specialCharacters.Contains(c))
                {
                    hasSpecialCharacter = true;
                    break;
                }
            }
            if (!hasSpecialCharacter)
                errors.Add("La contraseña debe contener al menos un carácter especial.");

            return errors;
        }

        private string[] GetRolesFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new InvalidOperationException("El token JWT no pudo ser leído correctamente.");
            }

            var roles = jsonToken?.Claims
                                .Where(claim => claim.Type == "role")
                                .Select(claim => claim.Value)
                                .ToArray();

            return roles ?? new string[0];
        }

        public async Task<Dictionary<int, int>> GetUsersCountByDistrict()
        {
            var usersWithAddresses = await _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Location)
                .ToListAsync();

            var usersWithValidLocations = usersWithAddresses
                .Where(u => u.Address != null && u.Address.Location != null)
                .ToList();

            var groupedUsers = usersWithValidLocations
                .GroupBy(u => u.Address.Location.LocationId)
                .ToList();

            var usersCountByDistrict = groupedUsers
                .ToDictionary(
                    g => g.Key != null ? g.Key : -1, // Si la clave es null, usamos -1 como clave en lugar de null
                    g => g.Count()
                );

            if(usersCountByDistrict == null)
            {
                throw new KeyNotFoundException("No se encontraron usuarios en los distritos.");
            }

            return usersCountByDistrict;
        }
        public async Task<List<UserDTO>> GetUsersWithoutBillReport()
        {
            var usersWithoutConsumption = await _context.Users
                                                .Include(u => u.Address)
                                                .Where(u => !u.ConsumptionBills.Any())
                                                .ToListAsync();


            if (usersWithoutConsumption == null)
            {
                throw new KeyNotFoundException("No se encontraron usuarios sin consumos.");
            }

            return _mapper.Map<List<UserDTO>>(usersWithoutConsumption); ;
        }

        public async Task<Dictionary<string, int>> GetUsersByRoleReport()
        {
            var report = new Dictionary<string, int>();

            var roleCounts = await _context.Users
                .GroupBy(u => u.Role.RoleName)
                .Select(g => new
                {
                    RoleName = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();

            foreach (var roleCount in roleCounts)
            {
                report.Add(roleCount.RoleName, roleCount.UserCount);
            }

            return report;
        }
    }
}