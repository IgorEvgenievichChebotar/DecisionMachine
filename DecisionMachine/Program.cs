namespace DecisionMachine;

public static class Program
{
    static void Main(string[] args)
    {
        Console.Write("Возраст кандидата: ");
        var age = Convert.ToInt16(Console.ReadLine());
        Console.Write("Опыт кандидата (месяцев): ");
        var experience = TimeSpan.FromDays(Convert.ToInt32(Console.ReadLine()) * 30);
        Console.Write("Рекомендации на кандидата (y/n): ");
        var referral = Convert.ToBoolean(Console.ReadLine() == "y");
        Console.Write("Работы кандидата (название срок, название срок): ");
        var workExperience = Console.ReadLine()!.Split(", ").ToList()
            .Select(s => new Work
            {
                Name = s.Split(" ")[0],
                Duration = TimeSpan.FromDays(Convert.ToInt32(s.Split(" ")[1]))
            }).ToList();
        Console.Write("Оконченное высшее кандидата (y/n): ");
        var completedHigherEdu = Convert.ToBoolean(Console.ReadLine() == "y");
        Console.Write("Навыки кандидата (навык, навык): ");
        var skills = Console.ReadLine()!.Split(", ").ToList();

        Console.WriteLine("----------------------------------------------------");

        var candidate = new Candidate
        {
            Age = age,
            Experience = experience,
            Referral = referral,
            WorkExperience = workExperience,
            CompletedHigherEdu = completedHigherEdu,
            Skills = skills
        };

        var decisionTopNode = DecisionTree();

        decisionTopNode.Evaluate(candidate);

        decisionTopNode.PrintTree();


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

    static void PrintTree(this DecisionQuery<Candidate> node, int depth = 0)
    {
        Console.WriteLine(new string(' ', depth * 4) + node.Title);
        if (node.Negative is DecisionQuery<Candidate> negative)
        {
            PrintTree(negative, depth + 1);
        }
        else
        {
            Console.WriteLine(new string('-', (depth + 1) * 4) + "Отказ");
        }
        if (node.Positive is DecisionQuery<Candidate> positive)
        {
            PrintTree(positive, depth + 1);
        }
        else
        {
            Console.WriteLine(new string('-', (depth + 1) * 4) + "Оффер");
        }
    }
}