using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfRegister()
        {
            TypeAdapterConfig<Course, CourseResponse>.NewConfig()
              .Map(dest => dest.Instructor, source => source.Instructor.UserName);

            TypeAdapterConfig<Course, CourseResponseForAdminStudent>.NewConfig()
      .Map(dest => dest.Name,
         src => src.Translations
             .FirstOrDefault(t =>
                 t.Language == MapContext.Current.Parameters["lang"].ToString()
             ).Name)

    .Map(dest => dest.Description,
         src => src.Translations
             .FirstOrDefault(t =>
                 t.Language == MapContext.Current.Parameters["lang"].ToString()
             ).Description);

        }
    }
}
