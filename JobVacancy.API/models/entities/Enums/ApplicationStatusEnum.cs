namespace JobVacancy.API.models.entities.Enums;

public enum ApplicationStatusEnum
{
    Submitted = 0, 
    UnderReview = 1, 
    Shortlisted = 2,
    Interviewing = 3,
    Evaluation = 4,
    OfferExtended = 5,
    Hired = 6,
    Rejected = 7,
    Withdrawn = 8
}