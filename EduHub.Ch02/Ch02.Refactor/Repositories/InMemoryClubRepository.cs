using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _store = new();

    public IReadOnlyList<Club> GetAll() => _store.AsReadOnly();
    public Club? GetById(Guid id) => _store.FirstOrDefault(club => club.Id == id);
    public void Add(Club club) => _store.Add(club);
}
