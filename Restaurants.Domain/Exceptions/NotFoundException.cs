namespace Restaurants.Domain.Exceptions;
public class NotFoundException : Exception {
    public string ResourceType { get; }
    public string ResourceIdentifier { get; }

    public NotFoundException(string resourceType, string resourceIdentifier)
        : base($"{resourceType} with id '{resourceIdentifier}' was not found.") {
        ResourceType = resourceType;
        ResourceIdentifier = resourceIdentifier;
    }
}
