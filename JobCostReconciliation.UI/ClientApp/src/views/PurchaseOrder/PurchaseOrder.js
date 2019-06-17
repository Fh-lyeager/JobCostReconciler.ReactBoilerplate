import React, { useState } from "react";
import {
  SortingState,
  PagingState,
  IntegratedPaging,
  IntegratedSorting
} from "@devexpress/dx-react-grid";
import {
  Grid,
  Table,
  PagingPanel,
  TableHeaderRow
} from "@devexpress/dx-react-grid-material-ui";

//import { communitiesRows, communitiesColumns } from "mockdata/communitiesData";

const Communities = () => {
  // TODO: Move to React Context
  const rows = communitiesRows;
  const columns = communitiesColumns;

  return (
    <React.Fragment>
      Dev Extreme React Grid{" "}
      <strong>
        https://devexpress.github.io/devextreme-reactive/react/grid/{" "}
      </strong>
      <Grid columns={columns} rows={rows}>
        <SortingState
          defaultSorting={[{ columnName: "community_code", direction: "asc" }]}
        />
        <PagingState />

        <IntegratedSorting />
        <IntegratedPaging />

        <Table />
        <TableHeaderRow showSortingControls />
        <PagingPanel pageSizes={20} />
      </Grid>
      <br />
      Total {rows.length}
    </React.Fragment>
  );
};

export default Communities;
