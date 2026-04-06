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

        public async Task<MatchResponse> ProcessMatchAsync(MatchRequest request)
        {
            var resumeText = await FileParser.ExtractText(request.ResumeFile);

            resumeText = resumeText.Length > 4000
                ? resumeText.Substring(0, 4000)
                : resumeText;

            resumeText = resumeText.Replace("\n", " ").Replace("\r", " ");

            var prompt = BuildPrompt(request.JobDescription, resumeText);

            var aiResult = await _aiService.GetMatchAnalysisAsync(prompt);

            return aiResult;
        }

        public async Task<MatchResponse> ProcessAsync(string jd, string resume)
        {
            var prompt = BuildPrompt(jd, resume);
            return await _aiService.AnalyzeAsync(prompt);
        }

        public async Task<MatchResult> MatchAsync(MatchRequest request)
        {
            var resumeText = await FileParser.ExtractText(request.ResumeFile);

            //resumeText = resumeText.Length > 4000
            //    ? resumeText.Substring(0, 4000)
            //    : resumeText;

            resumeText = resumeText.Replace("\n", " ").Replace("\r", " ");

            var messages = new List<Content>
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
                                    {request.JobDescription}

                                    Resume:
                                    {resumeText}
                                    "
                        }
                    }
                }
            };

            //return string.Empty;

            return await _aiService.MatchAsync(messages);
        }

        private string BuildPrompt(string jd, string resume)
        {
            return $@"
            You are an expert recruiter AI.

            Compare the Job Description and Resume.

            Return JSON:
            - Score (0-100)
            - Recommendation
            - Requirements breakdown
            - Summary

            Job Description:
            {jd}

            Resume:
            {resume}
            ";
        }

        private string BuildPrompt1(string jd, string resume)
        {
            return $@"
                You are an expert technical recruiter AI.

                Compare Job Description and Resume.

                Return STRICT JSON:

                {{
                  ""score"": number (0-100),
                  ""recommendation"": ""Strongly Recommend | Recommend | Borderline | Do Not Advance"",
                  ""requirements"": [
                    {{
                      ""requirement"": """",
                      ""status"": ""Met | Partial | Not Met"",
                      ""confidence"": ""High | Medium | Low"",
                      ""evidence"": """"
                    }}
                  ],
                  ""summary"": ""Explain in simple English for recruiter""
                }}

                Rules:
                - Recognize equivalent skills (AWS=Azure=GCP, React=Angular)
                - Evaluate experience depth
                - Avoid keyword-only matching

                JD:
                {jd}

                Resume:
                {resume}
                ";
        }
    }
}
