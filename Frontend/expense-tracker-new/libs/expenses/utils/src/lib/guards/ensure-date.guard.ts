import { ActivatedRoute, CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export function ensureDateGuard(): CanActivateFn {
  return () => {
    const router = inject(Router);
    const activatedRoute = inject(ActivatedRoute);
    const hasYear = activatedRoute.snapshot.params['year'];
    const hasMonth = activatedRoute.snapshot.params['month'];

    if (hasMonth && hasYear) {
      return true;
    }

    const date = new Date();

    return router.createUrlTree([
      `/expenses/${date.getFullYear()}/${date.getMonth() + 1}`,
    ]);
  };
}
