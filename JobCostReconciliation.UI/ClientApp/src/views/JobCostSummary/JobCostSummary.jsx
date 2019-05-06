import React from 'react';
// import PropTypes from "prop-types";
import Button from 'components/CustomButtons/Button.jsx';

import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardBody from "components/Card/CardBody.jsx";

class JobCostSummaryPage extends React.Component {
constructor(props, classes) {
    super(props, classes);
    this.state = { jobSummary: [], loading: true };
    this.getJobSummary = this.getJobSummary.bind(this);
    this.getJobSummaryByJob = this.getJobSummaryByJob.bind(this);
}

  getJobSummary() {
    fetch("api/JobCost/WorkflowJobCostSummary")
      .then(response => response.json())
      .then(data => {
        this.setState({ jobSummary: data, loading: false });
      });
  }

  getJobSummaryByJob(jobNumber) {
    fetch("api/JobCost/JobTotals/".concat(jobNumber))
      .then(response => response.json())
      .then(data => {
        this.setState({ jobTotals: data, loadingJobTotals: false });
      });
  }

  static renderJobCostSummary(jobSummary) {
    return (
      <CardBody>
        <table>
          <thead>
            <tr>
              <th>JobNumber</th>
              <th>Sapphire Total</th>
              <th>Pervasive Total</th>
            </tr>
          </thead>
          <tbody>
            {jobSummary.map(job => (
              <tr key={job.jobNumber}>
                <td>{job.jobNumber}</td>
                <td>{job.sapphireEgmTotal}</td>
                <td>{job.pervasiveEgmTotal}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </CardBody>
    );
  }

  render() {
    let jobCostSummary = this.state.jobSummary.length > 0 ? JobCostSummaryPage.renderJobCostSummary(this.state.jobSummary, this.props) : [];

    let classes = this.props;

    return (
      <div>
        <GridContainer>
          <GridItem xs={12} sm={12} md={12}>
            <Button
              className="Button"
              color="primary"
              onClick={() => {
                this.getJobSummary();
              }}
            >
              Get Job Cost Summary Report
            </Button>
            <Card>
              <CardHeader color="primary">
                <h4 className={classes.cardTitleWhite}>Job Cost Summary</h4>
              </CardHeader>
              {jobCostSummary}
            </Card>
          </GridItem>
        </GridContainer>
      </div>
    );
  }
}

export default JobCostSummaryPage;
