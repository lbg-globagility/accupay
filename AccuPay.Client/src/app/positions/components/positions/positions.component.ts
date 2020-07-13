import { Component, OnInit } from '@angular/core';
import { PositionService } from 'src/app/positions/position.service';
import { DivisionService } from 'src/app/divisions/division.service';
import { Division } from 'src/app/divisions/shared/division';
import { Position } from 'src/app/positions/shared/position';
import { FlatTreeControl } from '@angular/cdk/tree';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
} from '@angular/material/tree';
import { forkJoin } from 'rxjs';

interface Node {
  name: string;
  children?: Node[];
}

/** Flat node with expandable and level information */
interface FlatNode {
  expandable: boolean;
  name: string;
  items: number;
  level: number;
}

@Component({
  selector: 'app-positions',
  templateUrl: './positions.component.html',
  styleUrls: ['./positions.component.scss'],
})
export class PositionsComponent implements OnInit {
  divisions: Division[];

  positions: Position[];

  treeControl = new FlatTreeControl<FlatNode>(
    (node) => node.level,
    (node) => node.expandable
  );

  treeFlattener = new MatTreeFlattener(
    this.transformer,
    (node) => node.level,
    (node) => node.expandable,
    (node) => node.children
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  hasChild = (_: number, node: FlatNode) => node.expandable;

  constructor(
    private divisionService: DivisionService,
    private positionService: PositionService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  private transformer(node: Node, level: number): FlatNode {
    return {
      expandable: !!node.children && node.children.length > 0,
      items: node.children.length,
      name: node.name,
      level,
    };
  }

  private loadData(): void {
    const divisions$ = this.divisionService.getAll();
    const positions$ = this.positionService.getAll();

    forkJoin([divisions$, positions$]).subscribe(([divisions, positions]) =>
      this.buildTree(divisions, positions)
    );
  }

  private buildTree(divisions: Division[], positions: Position[]): void {
    const rootNodes: Node[] = [];
    const rootDivisions = divisions.filter((d) => d.parentId == null);

    for (const rootDivision of rootDivisions) {
      const rootNode = this.createDivisionNode(rootDivision);
      const childDivisions = divisions.filter(
        (d) => d.parentId === rootDivision.id
      );

      for (const childDivision of childDivisions) {
        const childNode = this.createDivisionNode(childDivision);

        childNode.children = positions
          .filter((p) => p.divisionId === childDivision.id)
          .map((p) => this.createPositionNode(p));

        rootNode.children.push(childNode);
      }

      rootNodes.push(rootNode);
    }

    this.dataSource.data = rootNodes;
  }

  private createDivisionNode(division: Division): Node {
    const node: Node = {
      name: division.name,
      children: [],
    };

    return node;
  }

  private createPositionNode(position: Position): Node {
    const node: Node = {
      name: position.name,
      children: [],
    };

    return node;
  }
}
