namespace EventServer;

internal sealed class PersonModel {
	public int Id { get; }
	public string Name { get; }
	public PersonModel(int id, string name) {
		this.Id = id;
		this.Name = name;
	}
}
