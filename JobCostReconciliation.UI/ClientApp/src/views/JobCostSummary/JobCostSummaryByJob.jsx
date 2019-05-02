import React, { Component } from 'react';
import PropTypes from "prop-types";
import Button from 'components/CustomButtons/Button.jsx';

import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardBody from "components/Card/CardBody.jsx";

class JobCostSearchPage extends React.Component {
    constructor(props, classes) {
        super(props, classes);
        this.state = { jobTotals: [], loading: true };
        this.getJobSummary = this.getJobSummary.bind(this);
    }

  getJobSummary(jobNumber) {
    var url = 'api/JobCost/JobTotals/'.concat(jobNumber);

      fetch(url)
          .then(response => response.json())
          .then(data => {
            this.setState({ jobTotals: data, loading: false });
          });
    }

    static renderJobSummary(jobTotals, classes) {
        return (
            <Card>
                <CardHeader color="primary">
                    <h4 className={classes.cardTitleWhite}>Job Cost Summary</h4>
                </CardHeader>
                <CardBody>
              <table width="100%">
                <thead>
                  <tr>
                    <th>JobNumber</th>
                    <th>Sapphire Total</th>
                    <th>Pervasive Total</th>
                  </tr>
                </thead>
                <tbody>
                  {jobTotals.map(job =>
                    <tr key={job.jobNumber}>
                      <td align="center">{job.jobNumber}</td>
                      <td align="center">{job.sapphireEgmTotal}</td>
                      <td align="center">{job.pervasiveEgmTotal}</td>
                    </tr>
                  )}
                </tbody>
              </table>
                </CardBody>
            </Card>
        )
    }

  render() {

    let jobTotalsByJob = JobCostSearchPage.renderJobSummary(this.state.jobTotals, this.props);
    let classes = this.props;

    return (
        <div>
          <input ref="jobNumberInput" type="text" placeholder="Job Number" />
          <Button
          className="Button"
          color="primary"
          onClick={() => {
            this.getJobSummary(this.refs.jobNumberInput.value);
            }
          }
          onSubmit={() => {
            this.getJobSummary(this.refs.jobNumberInput.value);
            }
          }>
          Search
          </Button>
          <GridContainer>
            <GridItem xs={12} sm={12} md={12}>
              {jobTotalsByJob}
            </GridItem>
          </GridContainer>
        </div>
    );
  }
}

export default (JobCostSearchPage);