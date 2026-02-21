using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abstraction.Database.Annotations;

namespace ArchAnge.ServiceDefaults.Sql;

public class DatabaseAttributesProvider
{
    public DatabaseAttributesProvider(Type type)
    {
        var tableAttribute = Attribute.GetCustomAttributes(type, typeof(TableAttribute)).SingleOrDefault() as TableAttribute;
        var props = type.GetProperties();
        var actions = new Dictionary<Type, Action<string, Attribute>>();

        TableName = tableAttribute?.Name ?? type.Name;
        actions.Add(typeof(DatabaseGeneratedAttribute), (p, a) => DatabaseGenerated.Add(p));
        actions.Add(typeof(ColumnAttribute), (p, a) => Aliases.Add(p, (ColumnAttribute)a));
        actions.Add(typeof(ForeignKeyAttribute), (p, a) => ForeignKeys.Add(p));
        actions.Add(typeof(KeyAttribute), (p, a) => Keys.Add(p));
        actions.Add(typeof(NotMappedAttribute), (p, a) => Properties.Remove(p));
        actions.Add(typeof(UniqueAttribute), (p, a) => Unique.Add(p));

        foreach (var property in props)
        {
            var attributes = property.GetCustomAttributes(false).ToDictionary(a => a.GetType(), a => (Attribute)a);

            Properties.Add(property.Name);

            foreach (var attribute in attributes)
            {
                if (actions.TryGetValue(attribute.Key, out Action<string, Attribute>? value))
                    value(property.Name, attribute.Value);
            }
        }
    }

    public IList<string> ForeignKeys { get; } = [];
    public IList<string> Unique { get; } = [];
    public IList<string> Properties { get; } = [];
    public IDictionary<string, ColumnAttribute> Aliases { get; } = new Dictionary<string, ColumnAttribute>();
    public IList<string> DatabaseGenerated { get; } = [];
    public IList<string> Keys { get; } = [];

    public string TableName { get; set; }

    public string Alias(string name)
    {
        Aliases.TryGetValue(name, out ColumnAttribute? value);
        return value?.Name ?? name;
    }
}

