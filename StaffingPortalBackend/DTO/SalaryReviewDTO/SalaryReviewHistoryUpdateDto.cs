using System;
using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewHistoryUpdateDto : SalaryReviewHistoryCreateDto
    {
        [Required]
        public int HistoryId { get; set; }
    }
}