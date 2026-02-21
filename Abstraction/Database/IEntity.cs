using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Database;

/// <summary>
/// Database entity
/// </summary>
public interface IEntity
{
}

/// <summary>
/// Database entities are Id generated
/// </summary>
/// <typeparam name="T"> Type of Id </typeparam>
public interface IEntity<out T> : IEntity
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Key]
    T Id { get; }
}

/// <summary>
/// <see cref="IEntity{T}"/> implementation
/// Instanciable entity, useful for request <see cref="Entity{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class Entity<T> : IEntity<T>
{
    /// <summary>
    /// Identifier
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required T Id { get; set; }
}
