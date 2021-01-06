using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CRUD_App.Services.Interfaces
{
   public interface INotification
    {
        DataTable GetNotificationForCustomer(int UserId, string ImagePath);
    }
}
