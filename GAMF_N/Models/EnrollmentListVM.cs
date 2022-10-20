using System.ComponentModel.DataAnnotations;

namespace GAMF_N.Models
{
    public class EnrollmentListVM
    {
        [Display(Name = "Tantárgy")]
        public string CourseTitle { get; set; }

        [Display(Name = "Hallgató")]
        public string StudentFullname { get; set; }

        [Display(Name = "Érdemjegy")]
        public string Grade { get; set; }

    }
}
