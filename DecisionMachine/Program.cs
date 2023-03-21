namespace DecisionMachine;

public class Program
{
    static void Main(string[] args)
    {
        var candidate = new Candidate
        {
            Age = 21,
            Experience = TimeSpan.FromDays(30 * 14),
            Referral = true,
            WorkExperience = new List<Work>
            {
                new() { Name = "ООО ВНИИЖТ", Duration = TimeSpan.FromDays(30 * 6) },
                new() { Name = "Фриланс", Duration = TimeSpan.FromDays(30 * 8) }
            },
            CompletedHigherEdu = true,
            Skills = new List<string> { "c#", "java", "spring", "asp.net" }
        };

        var decisionTopNode = DecisionTree();

        decisionTopNode.Evaluate(candidate);
    }

    static DecisionQuery<Candidate> DecisionTree()
    {
        var jumper = new DecisionQuery<Candidate>
        {
            Title = "Часто ли менял работу кандидат? (на каждой меньше 5 месяцев)",
            Test = c => c.WorkExperience.Any(w => w.Duration < TimeSpan.FromDays(30 * 5)),
            Negative = new DecisionResult { Result = false },
            Positive = new DecisionResult { Result = true }
        };

        var referral = new DecisionQuery<Candidate>
        {
            Title = "Есть ли рекомендации на этого кандидата?",
            Test = c => c.Referral,
            Negative = new DecisionResult { Result = false },
            Positive = jumper
        };

        var education = new DecisionQuery<Candidate>
        {
            Title = "Имеет ли кандидат оконченное высшее образование?",
            Test = c => c.CompletedHigherEdu,
            Negative = new DecisionResult { Result = false },
            Positive = referral
        };

        var skills = new DecisionQuery<Candidate>
        {
            Title = "Навыки кандидата соответствуют требованиям в вакансии?",
            Test = c => c.Skills.Contains("java") && c.Skills.Contains("c#"),
            Negative = referral,
            Positive = new DecisionResult { Result = true }
        };

        var workExpRequirements = new DecisionQuery<Candidate>
        {
            Title = "Опыт работы >= требованиям в вакансии?",
            Test = c => c.Experience >= TimeSpan.FromDays(365),
            Negative = education,
            Positive = skills
        };

        var workExp = new DecisionQuery<Candidate>
        {
            Title = "Имеет ли кандидат опыт работы?",
            Test = c => c.Experience > TimeSpan.Zero,
            Negative = new DecisionResult { Result = false },
            Positive = workExpRequirements
        };

        return workExp;
    }
}