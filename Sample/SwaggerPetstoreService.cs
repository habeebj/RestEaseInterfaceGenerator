[SerializationMethods(Query = QuerySerializationMethod.Serialized)]
public interface ISwaggerPetstoreService 
{
	[Get("v2/pet/findByStatus")]
	Task GetPetByStatus([Query] object status );

	[Get("v2/pet/findByTags")]
	Task GetPetByTags([Query] object tags );

	[Get("v2/store/inventory")]
	Task GetStore();

	[Get("v2/store/order/{orderId}")]
	Task GetStoreByOrderId([Path] object orderId );

	[Delete("v2/store/order/{orderId}")]
	Task DeleteStoreByOrderId([Path] object orderId );

	[Get("v2/user/login")]
	Task GetUserByUsernameAndPassword([Query] object username , [Query] object password );

	[Get("v2/user/logout")]
	Task GetUser();

}

public class ApiResponse 
{
	public int Code { get; set; }
	public string Type { get; set; }
	public string Message { get; set; }
}

public class Category 
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class Pet 
{
	public int Id { get; set; }
	public Category Category { get; set; }
	public string Name { get; set; }
	public List<string> PhotoUrls { get; set; }
	public List<Tag> Tags { get; set; }
	public string Status { get; set; }
}

public class Tag 
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class Order 
{
	public int Id { get; set; }
	public int PetId { get; set; }
	public int Quantity { get; set; }
	public DateTime ShipDate { get; set; }
	public string Status { get; set; }
	public boolean Complete { get; set; }
}

public class User 
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string Phone { get; set; }
	public int UserStatus { get; set; }
}


