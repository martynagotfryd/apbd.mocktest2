using mocktest2.Models.DTOs;

namespace mocktest2.Repositories;

public interface IRepository
{
    Task<bool> DoesDoctorExist(int idDoctor);
    Task<DoctorDto> GetDoctorInfo(int idDoctor);
}