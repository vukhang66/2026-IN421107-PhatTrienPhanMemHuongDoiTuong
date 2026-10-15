using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public interface IClubRepository
{
    IReadOnlyList<Club> GetAll();
    Club? GetById(Guid id);
    void Add(Club club);
}
