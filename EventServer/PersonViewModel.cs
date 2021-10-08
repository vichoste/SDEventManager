namespace EventServer;

internal sealed class PersonViewModel {
	private int _Id;
	private readonly List<PersonModel> _People;
	public int Count => this._People.Count;
	public PersonViewModel() => this._People = new();
	public PersonModel? this[int index] {
		get {
			if (this._People is not null) {
				var foundPerson = this._People is not null ? this._People[index] : null;
				if (foundPerson is not null) {
					return new(foundPerson.Id, foundPerson.Name);
				}
			}
			return null;
		}
	}
	public void AddPerson(string person) => this._People.Add(new PersonModel(this._Id++, person));
}
