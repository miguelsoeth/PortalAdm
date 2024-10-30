using PortalAdm.Core.Entities;

namespace PortalAdm.Core.DTOs;

public class ConsultaLoteMessage
{
    public List<string> Document { get; set; }
    public Product Product { get; set; }
    public User User { get; set; }
    public decimal Total { get; set; }
    public DateTime Date { get; private set; }
    

    public ConsultaLoteMessage(List<string> document, Product product, User user, decimal total)
    {
        Document = document;
        Product = product;
        User = user;
        Total = total;
        Date = DateTime.Now;
    }

    public void SetDate(DateTime date)
    {
        Date = date;
    }
}