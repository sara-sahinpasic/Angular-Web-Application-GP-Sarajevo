import { Component, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Router,
} from '@angular/router';

@Component({
  selector: 'app-driver-nav-page',
  templateUrl: './driver-nav-page.component.html',
  styleUrls: ['./driver-nav-page.component.scss'],
})
export class DriverNavPageComponent implements OnInit {
  protected routeHighlights: Map<string, boolean> = new Map<string, boolean>([
    ['delay', false],
    ['malfunction', false],
  ]);

  constructor(private router: Router, private route: ActivatedRoute) {}

  public async ngOnInit() {
    if (this.shouldRedirectToDriverDelay()) {
      await this.navigate('delay');
      return;
    }

    this.highlightMenuItem();
  }

  protected async navigate(path: string) {
    this.removeHightlightFromMenuItem();

    const isSuccessful: boolean | void = await this.router.navigate(
      [
        {
          outlets: { driver: path },
        },
      ],
      {
        relativeTo: this.route,
      }
    );

    if (this.shouldRedirectToDriverDelay()) {
      this.navigate('delay');
      return;
    }

    if (isSuccessful) {
      this.highlightMenuItem();
    }
  }

  private shouldRedirectToDriverDelay(): boolean {
    const routeChildren: ActivatedRouteSnapshot[] =
      this.route.snapshot.children;
    return routeChildren.length === 0;
  }

  private highlightMenuItem() {
    const activatedRoute: string = this.route.snapshot.children[0]?.url[0].path;
    this.routeHighlights[activatedRoute] = true;
  }

  private removeHightlightFromMenuItem() {
    const activatedRoute: string = this.route.snapshot.children[0]?.url[0].path;
    this.routeHighlights[activatedRoute] = false;
  }
}
