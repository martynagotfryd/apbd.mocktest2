namespace mocktest2.Models.DTOs;

public class PrescriptionMedDeleteDto
{
    public List<PrescriptionMedDeleteDto2> PrescriptionMedDeleteDto2s { get; set; }
}

public class PrescriptionMedDeleteDto2
{
    public int IdPrescription { get; set; }
}