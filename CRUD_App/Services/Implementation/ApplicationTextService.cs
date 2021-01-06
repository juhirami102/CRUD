using Go2Share.Core.Repositories;
using Go2Share.Core.UnitOfWork;
using Go2Share.Data.Entity;
using Go2Share.Entity.Entity;
using Go2Share.General.Resources.Services;
using Go2Share.Services.Interfaces;
using Go2Share.Services.ModelMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go2Share.Services.Implementation
{
    public class ApplicationTextService: IApplicationTextService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperFactory _mapperFactory;
        private readonly IResourceService _messageService;
        private IRepository<ApplicationText> _repository { get; set; }
        #endregion

        #region Constructor
        public ApplicationTextService(IUnitOfWork unitOfWork, IMapperFactory mapperFactory, IResourceService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapperFactory = mapperFactory;
            _repository = _unitOfWork.GetRepository<ApplicationText>();
            _messageService = messageService;
        }
        #endregion

        /// <summary>
        ///  Purpose : Get all Application Text
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApplicationTextEntity>> GetAllApplicationText()
        {
            var AllApplicationText = await _repository.GetAllAsync();
            if (AllApplicationText.Count() > 0)
                return _mapperFactory.GetList<ApplicationText, ApplicationTextEntity>(AllApplicationText);
            else
                return null;
        }

        /// <summary>
        /// Purpose : Get Application Text by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationTextEntity> GetApplicationTextById(int id)
        {
            var ApplicationText = await _repository.GetAsync(id);
            if (ApplicationText == null)
                return null;
            return _mapperFactory.Get<ApplicationText, ApplicationTextEntity>(ApplicationText);
        }
    }
}
