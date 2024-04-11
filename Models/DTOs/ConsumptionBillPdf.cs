using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                page.Header().Height(100).Background(Colors.Grey.Lighten1);
                page.Content().Background(Colors.Grey.Lighten3);
                page.Footer().Height(50).Background(Colors.Grey.Lighten1);
            });
        }
    }
}
