using Models.DTOs.Bill;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Models.DTOs
{
    public class ConsumptionBillPdf : IDocument
    {
        public ConsumptionBillDTO ConsumptionBill { get; }

        public ConsumptionBillPdf(ConsumptionBillDTO consumptionBill)
        {
            ConsumptionBill = consumptionBill;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);


                    page.Footer().Element(ComposeFooter);
                });
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {   // Header de la Factura
                    column.Item().Text($"Factura #{ConsumptionBill.ConsumptionBillId}").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Fecha de Facturación: ").SemiBold();
                        text.Span($"{ConsumptionBill.BillDate:d}");
                    });
                });

                string directory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
                directory = Path.Combine(directory, "assets", "Logo_negro.png");

                row.ConstantItem(205).Image(directory);
            });
        }

        void ComposeContent(IContainer container)
        {
            var subtityleStyle = TextStyle.Default.FontSize(15).SemiBold();

            container.PaddingVertical(40).Column(column =>
            {
                // Datos del usuario:
                column.Item().Row(row =>
                {
                    // Usuario:
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().BorderBottom(1).Text("Datos personales:").Style(subtityleStyle);
                        column.Item().Text(text =>
                        {
                            text.Span("Nombre y apellido: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.FullName}");
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("DNI: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.DniNumber}");
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Email: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.Email}");
                        });
                    });

                    row.ConstantItem(50);

                    // Dirección:
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().BorderBottom(1).Text("Dirección:").Style(subtityleStyle);
                        column.Item().Text(text =>
                        {
                            text.Span("Distrito: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.Address.Location.District.DistrictName}");
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Localidad: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.Address.Location.LocationName}");
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Barrio: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.Address.Neighborhood}");
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Calle: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.Address.StreetName}");
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Altura: ").SemiBold();
                            text.Span($"{ConsumptionBill.User.Address.StreetNumber}");

                        });
                    });

                });

                column.Spacing(5);

                // Tabla con detalles y Total:
                column.Item().Element(ComposeTable);
                var totalPrice = ConsumptionBill.BillDetails.Sum(x => x.PricePerUnit * x.UnitsConsumed);
                column.Item().AlignRight().Text($"Total: ${Math.Round(totalPrice, 2)}").FontSize(14);
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignCenter().Width(370).Height(50).Placeholder("Barcode");
                    column.Item().AlignRight().Text(x =>
                    {
                        // Número de página
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });
        }

        void ComposeTable(IContainer container)
        {
            container.PaddingVertical(20).Table(table =>
            {
                // Defino columnas de tabla
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                // Defino headers de columnas
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Servicio");
                    header.Cell().Element(CellStyle).AlignRight().Text("Precio por Unidad");
                    header.Cell().Element(CellStyle).AlignRight().Text("Consumo (u.)");
                    header.Cell().Element(CellStyle).AlignRight().Text("Subtotal");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // Creo celdas de tabla
                foreach (var detail in ConsumptionBill.BillDetails)
                {
                    table.Cell().Element(CellStyle).Text(ConsumptionBill.BillDetails.IndexOf(detail) + 1);
                    table.Cell().Element(CellStyle).Text(detail.Subscription.Service.ServiceName);
                    table.Cell().Element(CellStyle).AlignRight().Text($"${Math.Round(detail.PricePerUnit, 2)}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{Math.Round(detail.UnitsConsumed, 2)}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"${Math.Round(detail.PricePerUnit * detail.UnitsConsumed, 2)}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }

    }
}
