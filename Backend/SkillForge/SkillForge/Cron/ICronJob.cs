namespace SkillForge.Cron;

public interface ICronJob
{
    Task RunAsync();
}