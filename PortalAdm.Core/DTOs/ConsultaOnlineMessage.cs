using PortalAdm.Core.Entities;

namespace PortalAdm.Core.DTOs;

public class ConsultaOnlineMessage
{
    public string Document { get; set; }
    public Product Product { get; set; }
    public User User { get; set; }
    public DateTime Date { get; private set; }

    public ConsultaOnlineMessage(string document, Product product, User user)
    {
        Document = document;
        Product = product;
        User = user;
        Date = DateTime.Now;
    }

    public void SetDate(DateTime date)
    {
        Date = date;
    }
}