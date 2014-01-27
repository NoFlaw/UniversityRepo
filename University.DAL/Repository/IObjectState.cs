using System.ComponentModel.DataAnnotations.Schema;

namespace University.DAL.Repository
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}
