namespace DecisionMachine;

public class Candidate : IEntity
{
    public int Age { get; set; } // возраст соискателя
    public List<Work> WorkExperience { get; set; } // список работ, в которых работал соискатель
    public TimeSpan Experience { get; set; } // общий опыт работы соискателя
    public List<string> Skills { get; set; } // список навыков и компетенций соискателя
    public bool CompletedHigherEdu { get; set; } // уровень образования соискателя
    public bool Referral { get; set; } // наличие рекомендации от коллег или друзей
}

public class Work
{
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
}