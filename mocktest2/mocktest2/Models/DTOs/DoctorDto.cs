namespace mocktest2.Models.DTOs;

public class DoctorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<PrescriptionDto> Prescriptions { get; set; } = new List<PrescriptionDto>();
}

public class PrescriptionDto
{
    public int IdPrescription { get; set; } 
    public DateTime Date { get; set; } 
    public int IdPatient { get; set; } 
    public int IdMedicament { get; set; } 

}