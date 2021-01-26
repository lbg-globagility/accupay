import { MainComponent } from 'src/app/main/main.component';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { SidebarComponent } from 'src/app/main/sidebar/sidebar.component';
import { TopbarComponent } from 'src/app/main/topbar/topbar.component';

@NgModule({
  declarations: [MainComponent, SidebarComponent, TopbarComponent],
  imports: [SharedModule],
})
export class MainModule {}
