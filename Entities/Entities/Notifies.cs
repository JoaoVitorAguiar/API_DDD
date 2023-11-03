using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities;

public class Notifies
{
    public Notifies()
    {
        Notifications = new List<Notifies>();
    }
    [NotMapped]
    public string ProprietyName  { get; set; }
    [NotMapped]
    public string Message { get; set; }
    [NotMapped]
    public List<Notifies> Notifications { get; set; }


    public bool ValidateStringProperty(string value, string proprietyName)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(proprietyName)) 
        {
            Notifications.Add(new Notifies
            {
                Message = "Required field"
            });
            return false;
        }
        return true;
    } 
    public bool ValidateIntProperty(int value, string proprietyName)
    {
        if ( value < 1 || string.IsNullOrWhiteSpace(proprietyName)) 
        {
            Notifications.Add(new Notifies
            {
                Message = "Required field"
            });
            return false;
        }
        return true;
    }
}
