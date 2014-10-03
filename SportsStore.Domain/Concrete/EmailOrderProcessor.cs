using System;
using System.Net;
using System.Net.Mail;
using System.Text;

using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        //// ----------------------------------------------------------------------------------------------------------

        private readonly EmailSettings emailSettings;

        //// ----------------------------------------------------------------------------------------------------------

        public EmailOrderProcessor(EmailSettings settings)
        {
            this.emailSettings = settings;
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public void ProcessCart(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = this.emailSettings.UseSsl;
                smtpClient.Host = this.emailSettings.ServerName;
                smtpClient.Port = this.emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(this.emailSettings.Username, this.emailSettings.Password);

                if (this.emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = this.emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                var body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendLine(String.Format("{0} x {1} (subtotal: {2:C})", line.Quantity, line.Product.Name, subtotal));
                }

                body.AppendFormat("Total order value: {0:C}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? string.Empty)
                    .AppendLine(shippingInfo.Line3 ?? string.Empty)
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? string.Empty)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}", shippingInfo.GiftWrap ? "Yes" : "No");

                var mailMessage = new MailMessage(
                    this.emailSettings.MailFromAddress,
                    this.emailSettings.MailToAddress, 
                    "New order submitted!", 
                    body.ToString());

                if (this.emailSettings.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;

                smtpClient.Send(mailMessage);
            }
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}
