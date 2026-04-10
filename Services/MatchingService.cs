using AI.ResumeMatcher.Helpers;
using AI.ResumeMatcher.Models;
using AI.ResumeMatcher.Services.Interfaces;
using Google.GenAI.Types;

namespace AI.ResumeMatcher.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IAIService _aiService;

        public MatchingService(IAIService aiService)
        {
            _aiService = aiService;
        }

        public async Task<MatchResult> MatchAsync(MatchRequest request)
        {
            var resumeText = await FileParser.ExtractText(request.ResumeFile);

            resumeText = resumeText.Replace("\n", " ").Replace("\r", " ");

            var messages = BuildPrompt(request.JobDescription, resumeText.Trim());

            return await _aiService.MatchAsync(messages);
        }

        private List<Content> BuildPrompt(string jd, string resume)
        {
            return new List<Content>
            {
                new Content
                {
                    Role = "user",
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = @"
                            You are a senior IT hiring expert with 15+ years of experience.

                            Your task is to evaluate a resume against a job description.

                            Instructions:
                            - capture the candidate name
                            - Compare resume with JD
                            - Identify matching and missing skills
                            - Evaluate experience
                            - Be strict like a real hiring manager

                            Scoring:
                            - >=80 → PERFECT FIT
                            - 60–79 → GOOD FIT
                            - 40–59 → PARTIAL FIT
                            - <40 → NOT FIT

                            Return ONLY JSON:
                            {
                                ""candidateName"": """",
                                ""matchScore"": number,
                                ""decision"": ""PERFECT FIT | GOOD FIT | PARTIAL FIT | NOT FIT"",
                                ""matchingSkills"": [],
                                ""missingSkills"": [],
                                ""experienceSummary"": """",
                                ""gaps"": [],
                                ""reasoning"": [],
                                ""improvements"": []
                            }"
                        }
                    }
                },
                new Content
                {
                    Role = "user",
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = $@"
                                    Job Description:
                                    {jd}

                                    Resume:
                                    {resume}
                                    "
                        }
                    }
                }
            };
        }
    }
}
