export default function JobCostSummary()
{
    fetch('api/JobCost/WorkflowJobsEgmTotals')
        .then(response => response.json())
        .then(data => {
            this.setState({ jobSummary: Array.from(data) });
        });

    let jobSummary = this.state.jobSummary;

    return (
        {jobSummary}
    );
}
