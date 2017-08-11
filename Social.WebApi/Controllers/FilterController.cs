﻿using Framework.Core;
using Social.Application.AppServices;
using Social.Application.Dto;
using Social.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Social.WebApi.Controllers
{
    /// <summary>
    /// api about filters.
    /// </summary>
    [RoutePrefix("api/filters")]
    public class FilterController : ApiController
    {
        private IFilterAppService _appService;

        /// <summary>
        /// FilterController
        /// </summary>
        /// <param name="appService"></param>
        public FilterController(IFilterAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Get filter list.
        /// </summary>
        /// <returns></returns>
        [Route()]
        public List<FilterListDto> GetFilters()
        {
            return _appService.FindAll();
        }

        /// <summary>
        /// Get filter list for manging filters.
        /// </summary>
        /// <returns></returns>
        [Route("manage-filters")]
        public List<FilterManageDto> GetManegeFilters()
        {
            return _appService.FindManageFilters();
        }

        /// <summary>
        /// Get filter by id.
        /// </summary>
        /// <param name="id">filter id</param>
        /// <returns></returns>
        [Route("{id}", Name = "GetFilter")]
        public FilterDetailsDto GetFilter(int id)
        {
            return _appService.Find(id);
        }

        /// <summary>
        /// Create filter.
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [Route()]
        [ResponseType(typeof(FilterDetailsDto))]
        public IHttpActionResult PostFilter(FilterCreateDto createDto)
        {
            createDto = createDto ?? new FilterCreateDto();
            var filter = _appService.Insert(createDto);

            return CreatedAtRoute("GetFilter", new { id = filter.Id }, filter);
        }

        /// <summary>
        /// Update fitler.
        /// </summary>
        /// <param name="id">filter id</param>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [Route("{id}", Name = "PutFilter")]
        public FilterDetailsDto PutFilter(int id, FilterUpdateDto createDto)
        {
            createDto = createDto ?? new FilterUpdateDto();
            return _appService.Update(id, createDto);
        }

        /// <summary>
        /// Delete filter.
        /// </summary>
        /// <param name="id">filter id</param>
        /// <returns></returns>
        [Route("{id}")]
        public int DeleteFilter(int id)
        {
            _appService.Delete(id);
            return id;
        }

        /// <summary>
        /// Soritng filters.
        /// </summary>
        /// <param name="sortDtoList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sorting")]
        public IList<FilterManageDto> SortingFilters([Required]IList<FilterSortDto> sortDtoList)
        {
            if (!sortDtoList.Any())
            {
                throw SocialExceptions.BadRequest("sortDtoList is required.");
            }

            return _appService.Sorting(sortDtoList);
        }
    }
}