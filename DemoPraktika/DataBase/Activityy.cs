//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoPraktika.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Activityy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Activityy()
        {
            this.UserJuryActivities = new HashSet<UserJuryActivity>();
        }
    
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public int IdEvent { get; set; }
        public int DayOfEvent { get; set; }
        public System.TimeSpan TimeOfStart { get; set; }
        public int UserId { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserJuryActivity> UserJuryActivities { get; set; }
    }
}