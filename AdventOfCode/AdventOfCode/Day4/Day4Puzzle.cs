using System.Linq;

namespace AdventOfCode.Day4;

public static class Day4Puzzle
{
    public static int DetectSubsetsInCleaningAssignments(Assignment[] assignments)
    {
        return assignments.Where(OnePartnerHasSubsetOfSections).Count();
    }
    
    public static int DetectOverlapsInCleaningAssignments(Assignment[] assignments)
    {
        return assignments.Where(HasOverlappingSections).Count();
    }

    private static bool HasOverlappingSections(Assignment assignment)
    {
        return assignment.FirstPartnerSectionIds.Intersect(assignment.SecondPartnerSectionIds).Any();
    }

    private static bool OnePartnerHasSubsetOfSections(Assignment assignment)
    {
        return assignment.FirstPartnerSectionIds.All(assignment.SecondPartnerSectionIds.Contains) ||
               assignment.SecondPartnerSectionIds.All(assignment.FirstPartnerSectionIds.Contains);
    }
}

public record Assignment(SectionId[] FirstPartnerSectionIds, SectionId[] SecondPartnerSectionIds);

public record SectionId(int Id);