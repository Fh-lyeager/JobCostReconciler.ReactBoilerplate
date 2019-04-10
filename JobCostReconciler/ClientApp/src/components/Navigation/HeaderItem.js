import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default ({ item, onClick, ...remainingProps }) => (
  <li {...remainingProps}>
    <a title={item.name} onClick={onClick}>
      <span className="fa-layers fa-2x fa-fw">
        <FontAwesomeIcon icon="circle" />
        <span
          className="fa-layers-text fa-inverse"
          data-fa-transform="shrink-8 down-3"
          style={{ fontWeight: 900 }}
        >
          {item.abbreviation.substr(0, 2).toUpperCase()}
        </span>
      </span>
    </a>
  </li>
);
