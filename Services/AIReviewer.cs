using BlindMatchPAS_Final.Models;

namespace BlindMatchPAS_Final.Services
{
    public class AIReviewer
    {
        public (string feedback, int score) Evaluate(Project project)
        {
            int score = 0;
            string feedback = "";

            
            if (!string.IsNullOrEmpty(project.Abstract) && project.Abstract.Length > 50)
            {
                score += 30;
                feedback += "✔ Good detailed abstract.\n";
            }
            else
            {
                feedback += "⚠ Abstract is too short.\n";
            }

            
            if (!string.IsNullOrEmpty(project.TechStack))
            {
                score += 20;
                feedback += "✔ Tech stack clearly defined.\n";
            }
            else
            {
                feedback += "⚠ Missing tech stack.\n";
            }

            
            if (!string.IsNullOrEmpty(project.ResearchArea))
            {
                score += 20;
                feedback += "✔ Research area selected.\n";
            }
            else
            {
                feedback += "⚠ Research area missing.\n";
            }

            
            if (!string.IsNullOrEmpty(project.Abstract) &&
                (project.Abstract.ToLower().Contains("ai") ||
                 project.Abstract.ToLower().Contains("security") ||
                 project.Abstract.ToLower().Contains("system")))
            {
                score += 30;
                feedback += "✔ Strong technical concept detected.\n";
            }

            return (feedback, score);
        }
    }
}