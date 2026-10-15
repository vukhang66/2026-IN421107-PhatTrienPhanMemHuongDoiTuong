using EduHub.Ch02.Domain;

namespace Ch02.Refactor.Tests;

// Mẫu AAA: Arrange (chuẩn bị) - Act (thực hiện) - Assert (kiểm tra).
public class DomainTests
{
    [Fact]
    public void Club_Create_WithValidName_ReturnsClub()
    {
        // Arrange
        const string name = "Tech Club";
        const string description = "Câu lạc bộ công nghệ";

        // Act
        var club = Club.Create(name, description);

        // Assert
        Assert.NotEqual(Guid.Empty, club.Id);
        Assert.Equal(name, club.Name);
        Assert.Equal(description, club.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Club_Create_WithBlankName_ThrowsArgumentException(string name)
    {
        // Arrange: tên không hợp lệ được cung cấp qua tham số test.

        // Act
        var act = () => Club.Create(name, "mô tả");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Club_Create_WithNameLongerThanMaximum_ThrowsArgumentException()
    {
        // Arrange
        var name = new string('a', Club.MaxNameLength + 1);

        // Act
        var act = () => Club.Create(name, "mô tả");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Club_Create_WithMaximumLengthName_ReturnsClub()
    {
        // Arrange
        var name = new string('a', Club.MaxNameLength);

        // Act
        var club = Club.Create(name, "mô tả");

        // Assert
        Assert.Equal(name, club.Name);
    }

    [Theory]
    [InlineData(49, true)]
    [InlineData(50, false)]
    public void Club_HasRoom_AtCapacityBoundary_ReturnsExpectedResult(
        int currentMemberCount, bool expected)
    {
        // Arrange
        var club = Club.Create("Tech Club", "mô tả");

        // Act
        var hasRoom = club.HasRoom(currentMemberCount);

        // Assert
        Assert.Equal(expected, hasRoom);
    }

    [Fact]
    public void Event_Register_NewMember_ReturnsTrue()
    {
        // Arrange
        var ev = CreateEvent();
        var memberId = Guid.NewGuid();

        // Act
        var registered = ev.Register(memberId);

        // Assert
        Assert.True(registered);
        Assert.Contains(memberId, ev.RegisteredMemberIds);
    }

    [Fact]
    public void Event_Register_SameMemberTwice_ReturnsFalse()
    {
        // Arrange
        var ev = CreateEvent();
        var memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        var registeredAgain = ev.Register(memberId);

        // Assert
        Assert.False(registeredAgain);
        Assert.Single(ev.RegisteredMemberIds);
    }

    [Fact]
    public void Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = CreateEvent();

        // Act
        var act = () => ev.CheckIn(Guid.NewGuid());

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Event_CheckIn_RegisteredMember_RecordsCheckIn()
    {
        // Arrange
        var ev = CreateEvent();
        var memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        ev.CheckIn(memberId);

        // Assert
        Assert.Contains(memberId, ev.CheckedInMemberIds);
    }

    private static Event CreateEvent() =>
        new()
        {
            Id = Guid.NewGuid(),
            ClubId = Guid.NewGuid(),
            Title = "Workshop SOLID",
            Date = new DateTime(2026, 11, 20)
        };
}
