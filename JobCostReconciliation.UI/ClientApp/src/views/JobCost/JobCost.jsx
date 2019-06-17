import React from "react";
// import ReactDOM from "react-dom";
import Pagination from "react-js-pagination";
import PropTypes from "prop-types";
// @material-ui/core
import withStyles from "@material-ui/core/styles/withStyles";
import Icon from "@material-ui/core/Icon";
// @material-ui/icons
// import Store from "@material-ui/icons/Store";
// import Warning from "@material-ui/icons/Warning";
import DateRange from "@material-ui/icons/DateRange";
import Update from "@material-ui/icons/Update";
// import ArrowUpward from "@material-ui/icons/ArrowUpward";
// import AccessTime from "@material-ui/icons/AccessTime";
import Accessibility from "@material-ui/icons/Accessibility";
// import BugReport from "@material-ui/icons/BugReport";
import Code from "@material-ui/icons/Code";
import Queue from "@material-ui/icons/Queue";
import Security from "@material-ui/icons/Security";
// core components
import Button from "components/CustomButtons/Button.jsx";
import GridItem from "components/Grid/GridItem.jsx";
import GridContainer from "components/Grid/GridContainer.jsx";
// import Table from "components/Table/Table.jsx";
// import Tasks from "components/Tasks/Tasks.jsx";
import CustomTabs from "components/CustomTabs/CustomTabs.jsx";
// import Danger from "components/Typography/Danger.jsx";
import Card from "components/Card/Card.jsx";
import CardHeader from "components/Card/CardHeader.jsx";
import CardIcon from "components/Card/CardIcon.jsx";
import CardBody from "components/Card/CardBody.jsx";
import CardFooter from "components/Card/CardFooter.jsx";
// import WorkflowData from "functions/Workflow.jsx";
import sapphireIcon from "assets/img/sapphire_inner_graphic-01@36x36.png";
import dashboardStyle from "assets/jss/material-dashboard-react/views/dashboardStyle.jsx";
import "jquery/src/jquery";
require("jquery");
require("bootstrap-less");

class JobCostPage extends React.Component {
  state = {
    value: 0,
    workflows: [],
    loadingWorkflows: true,
    workflowQueue: [],
    loadingWorkflowQueue: true,
    sentryEvents: [],
    loadingSentryEvents: true,
    workflowJobSummary: [],
    loadingWorkflowJobSummary: true,
    jobData: [],
    loadingJobData: true,
    jobTotals: [],
    loadingJobTotals: true,
    activePage: 1
  };
  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleChangeIndex = index => {
    this.setState({ value: index });
  };

  handlePageChange(pageNumber) {
    this.setState({ activePage: pageNumber });
  }

  componentDidMount() {
    if (this.state.workflows.length === 0) {
      this.getWorkflowData();
    }

    if (this.state.workflowQueue.length === 0) {
      this.getWorkflowQueueItems();
    }

    if (this.state.workflowJobSummary.length === 0) {
      this.getWorkflowJobSummary();
    }

    if (this.state.sentryEvents.length === 0) {
      this.getSentryEvents();
    }
  }

  getWorkflowData() {
    fetch("api/Workflow")
      .then(response => response.json())
      .then(data => {
        this.setState({ workflows: data, loadingWorkflows: false });
      });
  }

  getWorkflowQueueItems() {
    fetch("api/WorkflowQueue")
      .then(response => response.json())
      .then(data => {
        this.setState({ workflowQueue: data, loadingWorkflowQueue: false });
      });
  }

  getSentryEvents() {
    fetch("api/JobCost/sentry/events")
      .then(response => response.json())
      .then(data => {
        this.setState({ sentryEvents: data, loadingSentryEvents: false });
      });
  }

  filterWorkflowQueue(workflowQueue, status) {
    return workflowQueue.filter(
      workflowQueue => status === workflowQueue.status
    );
  }

  getWorkflowJobSummary() {
    fetch("api/JobCost/WorkflowJobCostSummary")
      .then(response => response.json())
      .then(data => {
        this.setState({
          workflowJobSummary: data,
          loadingWorkflowJobSummary: false
        });
      });
  }

  getJobSummaryByJob(jobNumber) {
    fetch("api/JobCost/JobTotals/".concat(jobNumber))
      .then(response => response.json())
      .then(data => {
        this.setState({ jobTotals: data, loadingJobTotals: false });
      });
  }

  getJobSummaryByHomeRID(homeRID) {
    fetch("api/JobCost/JobData/".concat(homeRID))
      .then(response => response.json())
      .then(data => {
        this.setState({ jobData: data, loadingJobData: false });
      });
  }

  static renderSentryEventList(sentryEventList) {
    return (
      <tbody>
        {sentryEventList.map(item => (
          <tr key={item.eventID}>
            <td style={{ fontSize: 11 }}>
              <a href={item.title} target="_blank">
              {item.title.toUpperCase()}
              </a>
            </td>
            <td style={{ fontSize: 11 }}>
              {item.dateCreated}
            </td>
          </tr>
        ))}
      </tbody>
    );
  }

  static renderWorkflowQueue(workflowQueue, page_number = 1) {
    var page_size = 8;
    var queueData = workflowQueue.slice(
      (page_number - 1) * page_size,
      page_number * page_size
    );

    return (
      <tbody>
        {queueData.map(item => (
          <tr key={item.id}>
            <td>{item.id}</td>
            <td>{item.wFlowRID}</td>
            <td>{item.refObjRID}</td>
            <td>{item.status}</td>
          </tr>
        ))}
      </tbody>
    );
  }

  static renderJobCostSummary(jobSummary, page_number = 1) {
    var page_size = 8;
    var jobSummaryData = jobSummary.slice(
      (page_number - 1) * page_size,
      page_number * page_size
    );

    return (
      <tbody>
        {jobSummaryData.map(job => (
          <tr key={job.id}>
            <td onClick={() => {}}>{job.jobNumber}</td>
            <td>{job.sapphireEgmTotal}</td>
            <td>{job.pervasiveEgmTotal}</td>
            <td />
          </tr>
        ))}
      </tbody>
    );
  }

  render() {
    const { classes } = this.props;

    let numberWorkflows = this.state.loadingWorkflows ? "" : this.state.workflows.length;
    let queueData = this.state.loadingWorkflowQueue ? [] : JobCostPage.renderWorkflowQueue(this.state.workflowQueue);
    let queueDataCount = this.state.loadingWorkflowQueue ? "" : this.state.workflowQueue.length;
    let numberWorkflowsProcessed = this.state.loadingWorkflowQueue ? "" : this.state.workflowQueue.length;

    // let numberWorkflowsNew = this.state.loadingWorkflowQueue ? [] : this.filterWorkflowQueue(this.state.workflowQueue, "New").length;
    let numberWorkflowsError = this.state.loadingWorkflowQueue
      ? []
      : this.filterWorkflowQueue(this.state.workflowQueue, "Error").length;
    // let errorQueueData = this.state.loadingWorkflowQueue ? [] : JobCostPage.renderWorkflowQueue(this.filterWorkflowQueue(this.state.workflowQueue, "Error"));

    let workflowJobCostSummary =
      this.state.workflowJobSummary.length > 0
        ? JobCostPage.renderJobCostSummary(this.state.workflowJobSummary)
        : [];

    let jobTotals =
      this.state.jobTotals.length > 0
        ? JobCostPage.renderJobCostSummary(this.state.jobTotals)
        : [];

    let sentryEventList = this.state.loadingSentryEvents
      ? []
      : JobCostPage.renderSentryEventList(this.state.sentryEvents);

    return (
      <div>
        <GridContainer>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="warning" stats icon>
                <CardIcon color="warning">
                  <Icon>
                    <img src={sapphireIcon} alt="" />
                  </Icon>
                </CardIcon>
                <p className={classes.cardCategory}>Workflows Processed</p>
                <h3 className={classes.cardTitle}>
                  {numberWorkflowsProcessed}
                </h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Update />
                  Refresh
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="success" stats icon>
                <CardIcon color="success">
                  <Queue />
                </CardIcon>
                <p className={classes.cardCategory}>Items In Queue</p>
                <h3 className={classes.cardTitle}>{queueDataCount}</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <DateRange />
                  Last 24 Hours
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="danger" stats icon>
                <CardIcon color="danger">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    xlink="http://www.w3.org/1999/xlink"
                    width="167"
                    height="167"
                    viewBox="0 0 167 167"
                  >
                    <image
                      id="Layer_0"
                      data-name="Layer 0"
                      x="8"
                      y="16"
                      width="150"
                      height="134"
                      href="data:img/png;base64,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"
                    />
                  </svg>
                </CardIcon>
                <p className={classes.cardCategory}>Sentry Events</p>
                <h3 className={classes.cardTitle}>5</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Security />
                  <a href="#sentry.io" onClick={e => e.preventDefault()}>
                    Tracked from Sentry.io
                  </a>
                </div>
              </CardFooter>
            </Card>
          </GridItem>
          <GridItem xs={12} sm={6} md={3}>
            <Card>
              <CardHeader color="info" stats icon>
                <CardIcon color="info">
                  <Accessibility />
                </CardIcon>
                <p className={classes.cardCategory}>Errors</p>
                <h3 className={classes.cardTitle}>{numberWorkflowsError}</h3>
              </CardHeader>
              <CardFooter stats>
                <div className={classes.stats}>
                  <Update />
                  Just Updated
                </div>
              </CardFooter>
            </Card>
          </GridItem>
        </GridContainer>
        <GridContainer>
          <GridItem xs={12} sm={12} md={6}>
            <CustomTabs
              headerTitle="Workflow Queue"
              headerColor="primary"
              tabs={[
                {
                  tabName: "Queue",
                  tabIcon: Queue,
                  tabContent: (
                    <table width="100%">
                      <thead>
                        <tr>
                          <th align="left">Queue ID</th>
                          <th align="left">WorkflowRID</th>
                          <th align="left">RefObjRID</th>
                          <th align="left">Status</th>
                        </tr>
                      </thead>
                      {queueData}
                    </table>
                  )
                },
                {
                  tabName: "Exceptions",
                  tabIcon: Code,
                  tabContent: (
                    <table width="100%">
                      <thead>
                        <tr>
                          <th align="left" style={{  }}>
                            Detail
                          </th>
                          <th align="left" style={{}}>
                            Time
                          </th>
                        </tr>
                      </thead>
                      {sentryEventList}
                    </table>
                  )
                },
                {
                  tabName: "Processed",
                  tabIcon: Code,
                  tabContent: (
                    <CardBody>
                      <table width="100%" id="tableJobCostWorkflowsProcessed">
                        <thead>
                          <tr>
                            <th align="left" style={{ fontSize: 12 }}>
                              Job Number
                            </th>
                            <th align="left" style={{ fontSize: 12 }}>
                              Sapphire Total
                            </th>
                            <th align="left" style={{ fontSize: 12 }}>
                              Pervasive Total
                            </th>
                          </tr>
                        </thead>
                        {workflowJobCostSummary}
                      </table>
                      <Pagination
                        activePage={this.state.activePage}
                        itemsCountPerPage={8}
                        totalItemsCount={this.state.workflowJobSummary.length}
                        pageRangeDisplayed={5}
                        onChange={this.handlePageChange}
                      />
                    </CardBody>
                  )
                }
              ]}
            />
          </GridItem>
          <GridItem xs={12} sm={12} md={6}>
            <Card>
              <CardHeader color="warning">
                <h4 className={classes.cardTitleWhite}>Job Cost Details</h4>
                <div>
                  <input ref="jobNumberInput" type="text" size="15" />
                  <Button
                    className="Button"
                    color="primary"
                    size="sm"
                    onClick={() => {
                      this.getJobSummaryByJob(this.refs.jobNumberInput.value);
                    }}
                  >
                    Search
                  </Button>
                </div>
                <p className={classes.cardCategoryWhite} />
              </CardHeader>
              <CardBody>
                <table width="100%">
                  <thead>
                    <tr>
                      <th align="left">JobNumber</th>
                      <th align="left">Sapphire Total</th>
                      <th align="left">Pervasive Total</th>
                    </tr>
                  </thead>
                  {jobTotals}
                </table>
              </CardBody>
            </Card>
          </GridItem>
        </GridContainer>
      </div>
    );
  }
}

JobCostPage.propTypes = {
  classes: PropTypes.object.isRequired
};

export default withStyles(dashboardStyle)(JobCostPage);
