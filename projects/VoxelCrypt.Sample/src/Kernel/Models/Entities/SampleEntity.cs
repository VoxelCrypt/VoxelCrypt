using Kernel.Models.Enums;
using Kernel.Models.Exceptions;
using Kernel.Models.ValueObjects;

namespace Kernel.Models.Entities;

public sealed class SampleEntity
{
    private SampleEntity(SampleId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new SampleValidationException("Name cannot be empty.");
        }

        Id = id;
        Name = name.Trim();
        Status = SampleStatus.Alpha;
    }

    public static SampleEntity Create(string name)
    {
        return new SampleEntity(SampleId.New(), name);
    }

    public SampleId Id { get; }

    public string Name { get; }

    public SampleStatus Status { get; private set; }

    public void Activate()
    {
        Status = SampleStatus.Beta;
    }
}
