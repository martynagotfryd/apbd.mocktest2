using System.Data;
using Microsoft.Data.SqlClient;
using mocktest2.Models.DTOs;

namespace mocktest2.Repositories;

public class Repository : IRepository
{
    private readonly IConfiguration _configuration;

    public Repository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<bool> DoesDoctorExist(int idDoctor)
    {
        var query = "SELECT 1 FROM Doctor WHERE Doctor.IdDoctor = @IdDoctor";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdDoctor", idDoctor);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<DoctorDto> GetDoctorInfo(int idDoctor)
    {
        var query = @"SELECT d.FirstName AS FirstName, d.LastName AS LastName, " +
                    "p.IdPrescription AS IdPrescription, p.Date AS Date, p.IdPatient AS IdPatient, " +
                    "pm.IdMedicament AS IdMedicament " +
                    "FROM Doctor d " +
                    "JOIN Prescription p ON p.IdDoctor = d.IdDoctor " +
                    "JOIN Prescription_Medicament pm ON pm.IdPrescription = p.IdPrescription " +
                    "WHERE d.IdDoctor = @IdDoctor " +
                    "ORDER BY p.Date ";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdDoctor", idDoctor);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        var docFirstOrdinal = reader.GetOrdinal("FirstName");
        var docLastOrdinal = reader.GetOrdinal("LastName");
        var preIdOrdinal = reader.GetOrdinal("IdPrescription");
        var preDateOrdinal = reader.GetOrdinal("Date");
        var preIdPatientOrdinal = reader.GetOrdinal("IdPatient");
        var preMedIdMedOrdinal = reader.GetOrdinal("IdMedicament");

        DoctorDto doctorDto = null;

        while (await reader.ReadAsync())
        {
            if (doctorDto is not null)
            {
                doctorDto.Prescriptions.Add(new PrescriptionDto()
                {
                    IdPrescription = reader.GetInt32(preIdOrdinal),
                    Date = reader.GetDateTime(preDateOrdinal),
                    IdPatient = reader.GetInt32(preIdPatientOrdinal),
                    IdMedicament = reader.GetInt32(preMedIdMedOrdinal)
                });
            }
            else
            {
                doctorDto = new DoctorDto()
                {
                    FirstName = reader.GetString(docFirstOrdinal),
                    LastName = reader.GetString(docLastOrdinal),
                    Prescriptions = new List<PrescriptionDto>()
                    {
                        new PrescriptionDto()
                        {
                            IdPrescription = reader.GetInt32(preIdOrdinal),
                            Date = reader.GetDateTime(preDateOrdinal),
                            IdPatient = reader.GetInt32(preIdPatientOrdinal),
                            IdMedicament = reader.GetInt32(preMedIdMedOrdinal)
                        }
                    }
                };
            }
        }

        if (doctorDto is null) throw new Exception("ERROR WHILE GETTING DOCTOR INFO (NULL)");

        return doctorDto;
    }
}