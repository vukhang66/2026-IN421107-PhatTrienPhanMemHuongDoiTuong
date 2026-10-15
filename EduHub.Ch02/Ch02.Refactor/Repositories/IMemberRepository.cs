using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public interface IMemberRepository
{
    IReadOnlyList<Member> GetByClub(Guid clubId);
    Member? GetById(Guid id);
    void Add(Member member);
}
