import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"
import { CircleDollarSign, Home, List, Settings, Users } from "lucide-react"

const items = [
  {
    title: "Home",
    url: "/dash",
    icon: Home,
  },
  {
    title: "Pessoas",
    url: "/dash/pessoas",
    icon: Users,
  },
  {
    title: "Categorias",
    url: "/dash/categoria",
    icon: List,
  },
  {
    title: "Transações",
    url: "/dash/transacoes",
    icon: CircleDollarSign,
  },
  {
    title: "Configurações",
    url: "/dash/config",
    icon: Settings,
  },
]

export function AppSidebar() {
  return (
    <Sidebar>
      <SidebarHeader />
      <SidebarContent>
        <SidebarGroupLabel>Rotas da Aplicação</SidebarGroupLabel>
        <SidebarGroup>
          <SidebarMenu>
            {items.map((item) => (
              <SidebarMenuItem key={item.title}>
                <SidebarMenuButton asChild>
                  <a href={item.url}>
                    <item.icon />
                    <span>{item.title}</span>
                  </a>
                </SidebarMenuButton>
              </SidebarMenuItem>
            ))}
          </SidebarMenu>
        </SidebarGroup>
        <SidebarGroup />
      </SidebarContent>
    </Sidebar>
  )
}
