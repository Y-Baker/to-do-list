using AutoMapper;
using To_Do_List.DTOs;
using To_Do_List.Models;

namespace To_Do_List;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<AddTaskDTO, MyTask>();

        CreateMap<UpdateTaskDTO, MyTask>()
            .AfterMap((src, des, context) =>
            {
                DateOnly createdAt = (DateOnly?) context.Items["CreatedAt"] ?? throw new Exception("created at can't found");
                des.CreatedAt = createdAt;
            });
    }
}
