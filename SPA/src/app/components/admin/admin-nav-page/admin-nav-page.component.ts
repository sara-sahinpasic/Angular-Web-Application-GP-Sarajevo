import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';

@Component({
  selector: 'app-admin-nav-page',
  templateUrl: './admin-nav-page.component.html',
  styleUrls: ['./admin-nav-page.component.scss']
})
export class AdminNavPageComponent implements OnInit {

  protected routeHighlights: Map<string, boolean> = new Map<string, boolean>([
    ['users', false],
    ['company', false],
    ['report', false],
    ['home', false],
    ['tickets', false],
    ['news', false],
    ['routes', false],
    ['vehicles', false],
    ['holidays', false]
  ]);

  constructor(private router: Router, private route: ActivatedRoute) {}

  public async ngOnInit() {
    if (this.shouldRedirectToAdminHome()) {
      await this.navigate('home');
      return;
    }

    this.highlightMenuItem();
  }

  protected async navigate(path: string) {
    this.removeHightlightFromMenuItem();

    const isSuccessful: boolean | void = await this.router.navigate([{
      outlets: { admin: path }
    }],
    {
      relativeTo: this.route,
    });

    if (this.shouldRedirectToAdminHome()) {
      this.navigate('home');
      return;
    }

    if (isSuccessful) {
      this.highlightMenuItem();
    }
  }

  private shouldRedirectToAdminHome(): boolean {
    const routeChildren: ActivatedRouteSnapshot[] = this.route.snapshot.children;
    return routeChildren.length === 0
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
