using CRUD_App.Services.Interfaces;
using CRUD_App.General.Resources.Services;
using CRUD_App.Services.Interfaces;
using CRUD_App.Services.ModelMapper;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Data;
using CRUD_App.Core.UnitOfWork;
using CRUD_App.Entity.Entity;
using CRUD_App.Data.Entity;
using CRUD_App.Core.Repositories;

namespace CRUD_App.Services.Implementation
{
    public class NotificationService : INotification
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperFactory _mapperFactory;
        private readonly IResourceService _messageService;
       
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private IRepository<NotificationMaster> _repository { get; set; }
        #endregion


        #region Constructor
        public NotificationService(IUnitOfWork unitOfWork, IMapperFactory mapperFactory, IResourceService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapperFactory = mapperFactory;
            _repository = _unitOfWork.GetRepository<NotificationMaster>();
         
            _messageService = messageService;
            
        }
        #endregion
        public DataTable GetNotificationForCustomer(int UserId, string ImagePath)
        {
            string SPName = "STP_GoShare_GetNotificationForCustomer";

            List<string> ParamValue = new List<string>();
            ParamValue.Add(Convert.ToString(UserId));
            ParamValue.Add(Convert.ToString(ImagePath));
            List<string> ParamName = new List<string>();
            ParamName.Add("@UserId");
            ParamName.Add("@NotificationImagePath");
        
            return  _repository.GetDataBySP(SPName, ParamName, ParamValue);

        }
    }
}
