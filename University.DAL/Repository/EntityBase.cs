using System.ComponentModel.DataAnnotations.Schema;

namespace University.DAL.Repository
{
    public abstract class EntityBase : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; } //TODO: Renamed since a possible coflict with State entity column
    }


}
