using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Repositories;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _store = new();

    public IReadOnlyList<Member> GetByClub(Guid clubId) =>
        _store.Where(member => member.ClubId == clubId).ToList();

    public Member? GetById(Guid id) => _store.FirstOrDefault(member => member.Id == id);
    public void Add(Member member) => _store.Add(member);
}
